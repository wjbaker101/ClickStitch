# Clickstitch

The companion to track your cross-stitching progress and find fun digital patterns! From beginner to advanced options, you don't have to miss out.

## Development

The usual Vue commands:

```
yarn run serve
```

```
yarn run build
```

## HTTPS Certificates
These are to prevent the errors you get in the browser when you access a domain (i.e. `clickstitch.localdev`) when deving.

To create the required certificates you'll need to install [mkcert](https://github.com/FiloSottile/mkcert) and run the command:

```
mkcert "*.clickstitch.localdev" "clickstitch.localdev"
```