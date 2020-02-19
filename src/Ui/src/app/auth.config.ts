import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {

  // URL of the SPA to redirect the user after silent refresh
  silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',

  // set the scope for the permissions the client should request
  // The first three are defined by OIDC. The 4th is a usecase-specific one
  scope: 'api.read',

  strictDiscoveryDocumentValidation: false,
  // Url of the Identity Provider
  issuer: 'http://localhost:9000',
  skipIssuerCheck: true,
  requireHttps: false,
  // URL of the SPA to redirect the user to after login
  redirectUri: 'http://localhost:4200/auth-callback',

  // The SPA's id. The SPA is registerd with this id at the auth-server
  // clientId: 'server.code',
  clientId: 'angular_spa',

  disableAtHashCheck: true,
  // silentRefreshShowIFrame: true,

  showDebugInformation: true,

  sessionChecksEnabled: false,

  // timeoutFactor: 0.01,
};
