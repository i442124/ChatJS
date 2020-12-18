import { Component } from "react";
import { Redirect, Route } from "react-router-dom";

import { AuthService } from "./AuthorizeService";
import { ApplicationPaths, QueryParameterNames } from "./ApiAuthorizationConstants";

class AuthorizeRoute extends Component {

  state = {
    ready: false,
    authenticated: false
  }

  componentDidMount() {
    this.populateAuthenticationState();
  }

  render() {

    var link = document.createElement("a");
    link.href = this.props.path;

    const { ready, authenticated } = this.state;
    const returnUrl = `${link.protocol}//${link.host}${link.pathname}${link.search}${link.hash}`;
    const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURIComponent(returnUrl)}`;

    if (!ready) {
      return (<div></div>);
    }
    else {
      const { component: Component, ...rest } = this.props;
      return <Route {...rest} render={(props) => (
        authenticated === true
          ? <Component {...props} />
          : <Redirect to={redirectUrl} />
      )} />
    }
  }

  async populateAuthenticationState() {
    const user = await AuthService.getUser();
    this.setState({ ready: true, authenticated: !!user });
  }
}

export default AuthorizeRoute;
