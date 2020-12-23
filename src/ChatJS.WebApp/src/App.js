import React, { Component } from 'react';
import { Switch, Route } from 'react-router-dom';

import HomePage from './pages/HomePage';

import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Switch>
        <AuthorizeRoute exact path='/' component={HomePage} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Switch>
    );
  }
}

export default App;