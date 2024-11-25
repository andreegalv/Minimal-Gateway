
# Minimal API Gateway

Proyecto realizado en C#, describe una "Minimal API" para el uso como Gateway sencillo, fue desacoplado de otros proyectos realizados por mi persona, como "Puente" o balanceador de Microservicios.


## Descripción

La solución utiliza cuatro archivos escenciales.

1) authentication.json (Describe un archivo de authenticación JWT)
2) secrets.json (Describe un archivo de secretos)
3) endpoints.json (Describe un archivo de configuración o mapeo de endpoints)
4) cors.json (Desribe un archivo de configuración para Cors)

El uso es bastante sencillo. Se utiliza el archivo "endpoints.json" para mapear los endpoints o rutas permitidas, es un archivo inspirado en Ocelot.

Cors es un archivo para configurar multiples Cors en la solución, sin depender de código alguno.

Authentication.json y Secrets.json son archivos que describen la configuración de JWT, puede ser expandido para que contenga mas de una autenticación, pero a este nivel, solo es requerido JWT.

## Endpoints.json

Es el archivo primordial, y describe como es mapeado las rutas en el Gateway, se tomo como inspiración Ocelot.

"Upstream" es la sigla que indica todas las llamadas procedentes desde el cliente, es la que directamente es mapeada en el Gateway.

"Downstream" es la sigla que indica como sera transoformada la ruta procedente del cliente a los distintos microservicios definidos.

"Cors" indica si la ruta se le configura Cors (desde el cliente).

"RequiredHeaders" indica si la ruta fuerza que existan ciertos Headers desde el cliente, a su vez, los required headers seran enviados a los demas servicios (Forwarded).

"AuthenticationOptions" indica si utilizara autenticación en la ruta, por ahora no existe archivo dinamico para configurarlo, solo existe en formato JWT.

```json
  {
    "Routes": [
    {
        "UpstreamPathTemplate": "/gateway/security/v1/sigin/{*catchAll}",
        "UpstreamHttpMethod": [ "Get", "Post" ],
        "DownstreamPathTemplate": "/api/v1/sigin/{*catchAll}",
        "DownstreamServerOptions": {
            "Host": "localhost",
            "Port": 5001
        },
        "Cors": "ReactApp",
        "RequiredHeaders": [ "Accept", "Accept-Encoding", "Accept-Language" ]
    },
    {
        "UpstreamPathTemplate": "/gateway/security/v1/{*catchAll}",
        "UpstreamHttpMethod": [ "Get" ],
        "DownstreamPathTemplate": "/api/v1/{*catchAll}",
        "DownstreamServerOptions": {
            "Host": "localhost",
            "Port": 5001
        },
        "AuthenticationOptions": {
            "Scheme": "Bearer"
        },
        "Cors": "ReactApp",
        "RequiredHeaders": [ "Accept", "Accept-Encoding", "Accept-Language", "X-CustomerId", "X-DepartmentId", "X-ProjectId" ]
    }
    ]
  }
```
## Cors.json

```json
  {
    "Default": "ReactApp",
    "Cors": [
    {
        "Name": "ReactApp",
        "Origins": [ "http://localhost:3000" ],
        "Methods": [ "*" ],
        "Headers": [ "*" ],
        "ExposedHeaders": [ "X-Status-Error", "X-Exception-Type" ]
    }
    ]
  }
```
