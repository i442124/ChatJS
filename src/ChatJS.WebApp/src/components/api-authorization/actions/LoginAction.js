import { Component } from "react";
import { AuthenticationResultStatus, AuthService } from "../AuthorizeService";
import { ApplicationPaths, LoginActions, QueryParameterNames } from "../ApiAuthorizationConstants";

export class LoginAction extends Component {

  state = {
    message: undefined
  }

  componentDidMount() {
    const { name } = this.props;
    switch (name) {

      case LoginActions.Login:
        this.login();
        break;

      case LoginActions.LoginCallback:
        this.loginCallback();
        break;

      case LoginActions.LoginFailed:
        const params = new URLSearchParams(window.location.search);
        const error = params.get(QueryParameterNames.Message);
        this.setState({ message: error });
        break;

      case LoginActions.Register:
        this.redirectToRegister();
        break;

      case LoginActions.Profile:
        this.redirectToProfile();
        break;

      default:
          throw new Error(`Invalid action '${name}'`);
    }
  }

  render() {
    const { name } = this.props;
    const { message } = this.state;

    if (message) {
      return (<div>{message}</div>)
    }

    switch (name) {
      case LoginActions.Login:
      case LoginActions.LoginCallback:
        return (<div>Processing...</div>);

      case LoginActions.Profile:
      case LoginActions.Register:
        return (<div></div>);

      default:
        throw new Error(`Invalid action '${name}'`);
    }
  }

  async login() {
    const state = { returnUrl: this.getReturnUrl() };
    const result = await AuthService.signIn(state.returnUrl);

    switch (result.status) {
      case AuthenticationResultStatus.Success:
        this.navigateToUrl(state.returnUrl);
        break;
      case AuthenticationResultStatus.Fail:
        this.setState({  message: result.message});
        break;
      case AuthenticationResultStatus.Redirect:
        break;
      default:
        throw new Error(`Invalid status result '${result.status}'`);
    }
  }

  async loginCallback() {
    const state = { url: window.location.href };
    const result = await AuthService.signInCallback(state.url);

    switch (result.status) {
      case AuthenticationResultStatus.Success:
        this.navigateToUrl(this.getReturnUrl(result.state))
        break;
      case AuthenticationResultStatus.Fail:
        this.setState({ message: result.message });
        break;
      default:
        throw new Error(`Invalid authentication result status '${result.status}'.`);
    }
  }

  getReturnUrl(state) {
    const params = new URLSearchParams(window.location.search)
    const fromQuery = params.get(QueryParameterNames.ReturnUrl);
    if (fromQuery && !fromQuery.startsWith(`${window.location.origin}`)) {
       throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
    }

    return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`
  }

  redirectToProfile() {
    this.redirectToApiAuthorizationPath(ApplicationPaths.IdentityRegisterPath);
  }

  redirectToRegister() {
    this.redirectToApiAuthorizationPath(ApplicationPaths.IdentityRegisterPath)
  }
  
  redirectToApiAuthorizationPath(apiAuthroizationPath) {
    const redirectUrl = `${window.location.origin}/${apiAuthroizationPath}`;
    this.navigateToUrl(redirectUrl);
  }

  navigateToUrl(redirectUrl) {
    window.location.replace(redirectUrl);
  }
}