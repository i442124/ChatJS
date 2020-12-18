import React, { Component, Fragment } from 'react';
import { ApplicationPaths } from '../components/api-authorization/ApiAuthorizationConstants';
import { AuthService } from '../components/api-authorization/AuthorizeService';

import './HomePage.css';

class HomePage extends Component {

  render() {
    return (
      <Fragment>
        <h1>Home</h1>
        <button onClick={() => AuthService.signOut()}>Logout</button>
      </Fragment>
    );
  }
}

export default HomePage;