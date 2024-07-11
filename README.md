# CodeMode Token Service

CodeMode Token Service .NET layihələriniz üçün JWT tokenləri yaratmaq və idarə etmək üçün istifadə edilən bir paketdir. Bu paket, xüsusi token konfiqurasiyaları ilə işləmək, tokenlər yaratmaq və onları doğrulamaq üçün lazımi funksionallığı təmin edir.

## Quraşdırma

Paketinizi NuGet-dən quraşdırmaq üçün aşağıdakı əmrini istifadə edə bilərsiniz:

sh
dotnet add package CodeMode.Token.Service


## İstifadə

### Konfiqurasiya

Əvvəlcə `appsettings.json` faylınıza token konfiqurasiyalarını əlavə edin:

```
json
{
  "CustomTokenOptions": {
    "Audience": [ "your-audience" ],
    "Issuer": "your-issuer",
    "AccessTokenExpiration": 60,
    "RefreshTokenExpiration": 1440,
    "SecurityKey": "your-secure-key"
  }
}
```

### Xidmət Qeydiyyatı

Token xidmətini layihənizə qeydiyyatdan keçirmək üçün `Program.cs` faylınıza aşağıdakı sətirləri əlavə edin:

```
var builder = WebApplication.CreateBuilder(args);

// Token konfiqurasiyasını oxuyun
var tokenOptions = builder.Configuration.GetSection("CustomTokenOptions");

// Token xidmətini qeydiyyatdan keçirin
builder.AddTokenService(tokenOptions);

var app = builder.Build();
```

### Token Yaratma

Token yaratmaq üçün `TokenService` istifadə edin:

```
public class AuthService
{
    private readonly ITokenService<IdentityUser> _tokenService;
    private readonly CustomTokenOptions _tokenOptions;

    public AuthService(ITokenService<IdentityUser> tokenService, IOptions<CustomTokenOptions> tokenOptions)
    {
        _tokenService = tokenService;
        _tokenOptions = tokenOptions.Value;
    }

    public async Task<CreateTokenDto> GenerateTokenAsync(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        return await _tokenService.CreateTokenAsync(user, claims, _tokenOptions);
    }
}
```

### Nümunə Proqram

```
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Token konfiqurasiyasını oxuyun
        var tokenOptions = builder.Configuration.GetSection("CustomTokenOptions");

        // Token xidmətini qeydiyyatdan keçirin
        builder.AddTokenService(tokenOptions);

        var app = builder.Build();

        var userManager = app.Services.GetRequiredService<UserManager<IdentityUser>>();
        var tokenService = app.Services.GetRequiredService<ITokenService<IdentityUser>>();
        var tokenOptionsValue = app.Services.GetRequiredService<IOptions<CustomTokenOptions>>().Value;

        var user = new IdentityUser { UserName = "testuser", Email = "testuser@example.com" };
        await userManager.CreateAsync(user, "Password123!");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = await tokenService.CreateTokenAsync(user, claims, tokenOptionsValue);

        Console.WriteLine($"Access Token: {token.AccessToken}");
        Console.WriteLine($"Refresh Token: {token.RefreshToken}");

        app.Run();
    }
}
```

## Lisenziya

Bu proyekt MIT lisenziyası ilə lisenziyalanmışdır. Ətraflı məlumat üçün [LICENSE](LICENSE) faylını baxa bilərsiniz.
