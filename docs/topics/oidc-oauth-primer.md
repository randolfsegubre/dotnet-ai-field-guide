# Topic 7: OpenID Connect (OIDC) and OAuth 2.0 primer

Category: Security & Identity
Status: Planned (concept-only topic, no code expected)

## What it is

**OAuth 2.0** lets an application get limited access to a user's data or actions on another
service, without ever seeing that user's password. **OpenID Connect (OIDC)** is an identity
layer built on top of OAuth 2.0 that lets an application confirm *who* a user is (not just what
they're allowed to do) via a trusted identity provider.

## How it's used in practice

The user logs in at the identity provider (not your app). Your app receives a code, which it
exchanges for tokens. Those tokens prove who the user is (OIDC) and/or what the app is allowed
to do on the user's behalf (OAuth 2.0).

## Best use cases, and when to use it

| Use OIDC/OAuth 2.0 when | JSON Web Token (JWT) alone is enough when |
|---|---|
| Users should log in via an external identity provider (Google, Microsoft Entra ID, a company Single Sign-On (SSO)) | Your own app issues and validates its own tokens for its own users, no external identity provider involved |

## Where it's implemented here

Not applicable, concept-only topic. Existing evidence: JSON Web Token authentication with
token refresh, built in personal projects, but not OIDC or SSO in production. This doc exists
to hold the concept-level understanding honestly, without overstating it as hands-on experience.

## Gotchas actually hit

Not applicable, concept-only topic.

## Exercise

Not applicable, concept-only topic.
