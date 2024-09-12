using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// NO HAGAS LO QUE ESTOY A PUNTO DE HACER
var key = Encoding.UTF8.GetBytes("ClaveSuperSecretaParaFirmarJWT34786347667432");

builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost:5018",
        ValidAudience = "localhost:5018",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

/**
    POLITICA DE CORS DE SI A TODO
builder.Services.AddCors(options =>
{
    options.AddPolicy("YesAll", policy =>{
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
*/

//  POLITICA DE CORS RESTRICTIVA
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("SpecificPolicy", policy =>{
        policy.WithOrigins("https://google.com")
            .WithMethods("GET", "POST")
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
            .WithHeaders("Authorization");
    });
});*/

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseCors("SpecificPolicy");

app.UseAuthorization();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();