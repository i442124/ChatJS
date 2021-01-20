import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './UserArea.css';

class UserArea extends Component {

  state = {
    ready: false,
    user: undefined,
  }

  componentDidMount() {
    this.fetchComponentData();
  }

  async fetchComponentData() {

    const request = `api/protected/users`;
    console.log('UserArea', { request });

    const response = await AuthService.fetch(request);
    console.log('UserArea', { response });

    const data = await response.json();
    console.log('UserArea', { data });

    this.setState({ user: data, ready: true });
    const { componentDataChanged } = this.props;
    !!componentDataChanged && componentDataChanged(data);
  }

  render() {

    const { user } = this.state;
    const { component: Component } = this.props;

    return (
      <div aria-label="user-area">
        <div className="d-flex align-items-center p-2">
          <div className="flex-grow-1">
            <Component {...user} />
          </div>
          <div className="flex-grow-0">
            <button className="btn btn-dark" onClick={() => AuthService.signOut()}>Logout</button>
          </div>
        </div>
      </div>
    );
  }
}

export default UserArea;