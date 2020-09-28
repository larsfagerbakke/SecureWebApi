# Secure Web Api

This is a serverless base project. **WORK IN PROGRESS**

## Description

This is a basis for a secure serverless web api. Firstly, it's a basic implementation, meaning it's supposed to be expanded on. It holds simple implementations to get a secure web api up and running, but it's not complete. Second; it's serverless. It's done with Azure Functions V3. I know; cloud... serverless... pretty hip to the hop.

## Getting started

In order to get this to work there needs to be file called "*local.settings.json*" in SecureWebApi folder, add the following:

```
"jwt": {
    "jwtKey": "key",
    "jwtIssuer": "issuer",
    "jwtAudience": "audience"
  }
```

... and replace the values with your values :-).

## Todo

- Tests
- ... other stuff?

#### Contribute?

Feel free to drop me a message. Mail on contact page. Create pull request :-).