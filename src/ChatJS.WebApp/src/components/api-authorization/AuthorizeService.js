import { UserManager, WebStorageStateStore } from 'oidc-client';
import { ApplicationPaths, ApplicationName } from './ApiAuthorizationConstants';

class AuthorizeService {

  async getUser() {
    await this.ensureUserManagerInitialized();
    return await this._userManager.getUser();
  }

  async signIn(returnUrl) {

    await this.ensureUserManagerInitialized();
    const args = { useReplaceToNavigate: true, data: returnUrl };

    try {
      await this._userManager.signinSilent(args);
      return { status: AuthenticationResultStatus.Success }
    } catch (silentSignInError) {
      console.log('Silent login failed, login required');

      try {
        await this._userManager.signinRedirect(args);
        return { status: AuthenticationResultStatus.Redirect }
      } catch (redirectSignInError) {
        console.log(`There was an error trying to log in '${redirectSignInError}'`);
        return { status: AuthenticationResultStatus.Fail, message: redirectSignInError };
      }
    }
  }

  async signInCallback(callbackUrl) {
    try {
      await this.ensureUserManagerInitialized();
      const user = await this._userManager.signinCallback(callbackUrl);
      return { status: AuthenticationResultStatus.Success, state: user };
    } catch (signInCallbackError) {
      console.log('There was an error signing in: ', signInCallbackError);
      return { status: AuthenticationResultStatus.Fail, message: signInCallbackError };
    }
  }

  async signOut(returnUrl) {
    await this.ensureUserManagerInitialized();
    const args = { useReplaceToNavigate: true, data: returnUrl };

    try {
      await this._userManager.signoutRedirect(args);
      return { status: AuthenticationResultStatus.Redirect };
    } catch (redirectSignOutError) {
      console.log(`There was an error trying to log out '${redirectSignOutError}'`)
      return  { status: AuthenticationResultStatus.Fail, message: redirectSignOutError }
    }
  }

  async signOutCallback(callbackUrl) {
    try {
      await this.ensureUserManagerInitialized();
      const response = await this._userManager.signoutCallback(callbackUrl);
      return { status: AuthenticationResultStatus.Success, state: response };
    } catch (signOutCallbackError) {
      console.log(`There was an error trying to log out '${signOutCallbackError}'.`);
      return { status: AuthenticationResultStatus.Fail, message: signOutCallbackError };
    }
  }

  async ensureUserManagerInitialized() {
    if (this._userManager === undefined) {

      var response = await fetch(ApplicationPaths.ApiAuthorizationClientConfigurationUrl);
      if (response.ok) {

        var settings = await response.json();
        settings.automaticSilentRenew = true;
        settings.includeIdTokenInSilentRenew  = true;
        settings.userStore = new WebStorageStateStore({
          prefix: ApplicationName
        });

        this._userManager = new UserManager(settings);
      }
    }
  }
}

export const AuthService = new AuthorizeService();
export const AuthenticationResultStatus = {
  Redirect: 'redirect',
  Success: 'success',
  Fail: 'fail'
}