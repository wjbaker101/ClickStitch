# Clickstitch

_The_ cross-platform companion to track your cross-stitching progress, managing inventory and finding fun digital patterns!
From beginner to advanced options, you don't have to miss out.

### [clickstitch.app](https://clickstitch.app)

## Development

Run dev server/ build:

`yarn run dev`
`yarn run build`

### backend/appsecrets[.Development].json

```json lines
{
    "Database": {
        "Host": string,
        "Port": number,
        "Database": string,
        "Username": string,
        "Password": string
    },
    "Cloudinary": {
        "CloudName": string,
        "ApiKey": string,
        "ApiSecret": string
    },
    "Auth": {
        "LoginToken": {
            "SecretKey": string
        },
        "Password": {
            "Pepper": string
        }
    },
    "Inkwell": {
        "BaseUrl": string
    }
}
```