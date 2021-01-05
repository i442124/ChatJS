import React, { Component } from 'react';

import './Banner.css';
import logo from '../assets/img/logo.png';

class Banner extends Component {

  styles = {
    width: this.props.width || undefined,
    height: this.props.height || undefined,
  }

  render() {
    return (
      <div className="banner d-flex" style={this.styles}>
        <img className="img-fluid h-100" src={logo} alt='logo'/>
      </div>
    );
  }
}

export default Banner;