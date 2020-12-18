import { Component } from "react";
import { AuthenticationResultStatus, AuthService } from "../AuthorizeService";
import { LogoutActions, QueryParameterNames } from "../ApiAuthorizationConstants";

export class LogoutAction extends Component {

  state = {
    isReady: false,
    message: undefined,
    authenticated: false
  }

  componentDidMount() {
    const { name } = this.props;
    switch (name) {

      case LogoutActions.Logout:
        this.logout();
        break;

      case LogoutActions.LogoutCallback:
        this.logoutCallback();
        break;

      case LogoutActions.LoggedOut:
        this.setState({ isReady: true, message: "You successfully logged out!" });
        break;
    }
  }

  render() {
    const { name } = this.props;
    const { message, isReady } = this.state;

    if (!isReady) {
      return (<div></div>)
    }

    if (!!message) {
      return (<div>{message}</div>)
    }

    switch (name) {
      case LogoutActions.Login:
      case LogoutActions.LoginCallback:
        return (<div>Processing...</div>);
    }
  }

  async logout() {
    if (AuthService.getUser()) {
      const state = { returnUrl: this.getReturnUrl() };
      const result = await AuthService.signOut(state.returnUrl);

      switch (result.status) {
        case AuthenticationResultStatus.Success:
          this.navigateToUrl(state.returnUrl);
          break;
        case AuthenticationResultStatus.Fail:
          this.setState({ message: result.message});
          break;
        case AuthenticationResultStatus.Redirect:
          break;
        default:
          throw new Error(`Invalid authentication result status '${result.status}'.`);
      }
    }
    else {
      this.setState({ message: "You successfully logged out!" });
    }
  }

  async logoutCallback() {

    const state = { url: window.location.href };
    const result = await AuthService.signOutCallback(state.url);
    console.log(result);

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

  navigateToUrl(redirectUrl) {
    window.location.replace(redirectUrl);
  }
}