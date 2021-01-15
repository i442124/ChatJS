export const QueryParameterNames = {
  ReturnUrl: 'returnUrl',
  Message: 'message'
}

export const LoginActions = {
  Login: 'login',
  LoginFailed: 'login-failed',
  LoginCallback: 'login-callback',
  Profile: 'profile',
  Register: 'register'
}

export const LogoutActions = {
  Logout: 'logout',
  LoggedOut: 'logged-out',
  LogoutCallback: 'logout-callback'
}

const prefix = '/authentication';

export const ApplicationBaseAddress = 'https://localhost:3002';
export const ApplicationName = 'ChatJS.WebApp';
export const ApplicationPaths = {

  DefaultLoginRedirectPath: '/',
  ApiAuthorizationClientConfigurationUrl: `/_configuration/${ApplicationName}`,
  ApiAuthorizationPrefix: prefix,

  Login: `${prefix}/${LoginActions.Login}`,
  LoginFailed: `${prefix}/${LoginActions.LoginFailed}`,
  LoginCallback: `${prefix}/${LoginActions.LoginCallback}`,
  Profile: `${prefix}/${LoginActions.Profile}`,
  Register: `${prefix}/${LoginActions.Register}`,

  LogOut: `${prefix}/${LogoutActions.Logout}`,
  LoggedOut: `${prefix}/${LogoutActions.LoggedOut}`,
  LogOutCallback: `${prefix}/${LogoutActions.LogoutCallback}`,

  IdentityManagePath: 'Identity/Account/Manage',
  IdentityRegisterPath: 'Identity/Account/Register'
};
