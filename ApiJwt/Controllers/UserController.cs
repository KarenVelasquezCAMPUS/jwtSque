using ApiJwt.Dtos;
using ApiJwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiJwt.Controllers;

public class UserController : ApiBaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    //  registrar nuevos usuarios
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterAsync(RegisterDto model) 
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);  // RegisterResponseDto contiene el resultado del registro, el método de acción RegisterAsync() devuelve un objeto RegisterResponseDto a través del atributo Ok().
    }

    // genera un token jwt para un usuario autenticado
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result); // GetTokenResponseDto contiene el token jwt, " ".
    }

    // agrega un nuevo rol a un usuario
    [HttpPost("addrole")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result); // AddRoleResponseDto que contiene el resultado de la operación, " 
    }

    // token refresh
    [HttpPost("refreshtoken")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }

    // se establece el token de actualización en una cookie HTTP.
    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    /* 
    // pager

    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<PaisxDepaDto>>> Get11([FromQuery] Params paisParams)
    {
        var pais = await unitofwork.Paises.GetAllAsync(paisParams.PageIndex, paisParams.PageSize, paisParams.Search);
        var lstPaisesDto = mapper.Map<List<PaisxDepaDto>>(pais.registros);
        return new Pager<PaisxDepaDto>(lstPaisesDto, pais.totalRegistros, paisParams.PageIndex, paisParams.PageSize, paisParams.Search);
    } 

    // params 

    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<Pager<PaisxDepaDto>>> Get11([FromQuery] Params paisParams)
    {
        var pais = await unitofwork.Paises.GetAllAsync(paisParams.PageIndex, paisParams.PageSize, paisParams.Search);
        var lstPaisesDto = mapper.Map<List<PaisxDepaDto>>(pais.registros);
        return new Pager<PaisxDepaDto>(lstPaisesDto, pais.totalRegistros, paisParams.PageIndex, paisParams.PageSize, paisParams.Search);
    }
    */

}
