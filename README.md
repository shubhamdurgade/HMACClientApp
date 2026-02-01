# HMACClientApp

Lightweight .NET 8 console client demonstrating HMAC-SHA256 authorization for HTTP APIs.

This repository contains:
- HMACHelper.cs — helper for generating HMAC authorization tokens and sending HTTP requests.
- Program.cs — sample console client that demonstrates POST, GET (all / by id), and DELETE requests against an API.

Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or any editor that supports .NET 8 / C# 12
- Network access to the target API (example uses https://localhost:7063)

Quick start (CLI)
1. Clone the repo and open a terminal in the solution folder.
2. Build:
   dotnet build
3. Run:
   dotnet run --project <ProjectName>   (or run from Visual Studio)

Open in Visual Studio 2022
- Open the solution, set the console project as startup, and press F5 or __Debug > Start Debugging__.

Configuration
- Program.cs contains demo values for:
  - clientId (demo)
  - secretKey (demo) — replace with a secure value.
  - baseUrl — API base URL (example: https://localhost:7063)

Important: do NOT commit production secrets. Use secrets management (environment variables, __User Secrets__ for local dev, or a secure vault) in real projects.

How HMAC is generated
- The HMAC token format added to the Authorization header:
  HMAC {clientId}|{token}|{nonce}|{timestamp}
- token is Base64(HMAC-SHA256(secretKey, payload))
- payload is built as: METHOD + PATH + nonce + timestamp (+ request body when METHOD is POST or PUT)
- nonce (GUID) and timestamp (UTC seconds) prevent replay attacks.

Files of interest
- HMACHelper.cs
  - GenerateHmacToken(...) — constructs payload, computes HMAC-SHA256 and returns header value.
  - SendRequestAsync(...) — serializes JSON body (when provided), composes HttpRequestMessage, sets Authorization header using TryAddWithoutValidation, and sends via HttpClient.
- Program.cs
  - Demo client showing create (POST), list (GET), get-by-id (GET), and delete (DELETE) flows with console outputs.

Security notes
- The sample currently hard-codes clientId/secretKey for demonstration only. Replace with a secure secret source before use.
- Verify server-side validation of nonce/timestamp and use TLS (HTTPS).

Short commit summary (suggested)
- feat(hmac): add HMAC helper for token generation and request sender; add sample console client demonstrating POST/GET/DELETE flows

If you want, I can:
- Add a small README section with example curl requests using the generated Authorization header.
- Move secret configuration to environment variables or __User Secrets__ and update Program.cs accordingly.