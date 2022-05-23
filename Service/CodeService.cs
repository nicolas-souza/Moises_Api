
using Newtonsoft.Json;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using api.Database.Models;

#pragma warning disable

public class CodeService
{  
    const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

public static string Encoder(DtoDecoderUsuario payload)
{
    IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
    IJsonSerializer serializer = new JsonNetSerializer();
    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

    var token = encoder.Encode(payload, secret);

    return token;
}

public static DtoDecoderUsuario Decoder(string? token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, secret, verify: true);

                var result = JsonConvert.DeserializeObject<DtoDecoderUsuario>(json);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
            

            return null;
        }
}