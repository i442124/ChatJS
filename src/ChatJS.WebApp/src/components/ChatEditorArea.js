import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './ChatEditorArea.css';
import './ChatListEntry.css';

class ChatEditorArea extends Component {

  state = {
    ready: false,
    users: undefined
  }

  componentDidMount() {
    this.fetchComponentData();
  }

  async fetchComponentData() {

    const request = `api/protected/users/all`;
    console.log('ChatEditorArea', request);

    const response = await AuthService.fetch(request);
    console.log('ChatEditorArea', response);

    const data = await response.json();
    console.log('ChatEditorArea', data);

    const users = data.users.map(user => ({ ...user, selected: false }));
    this.setState({ ...data, users: users, ready: true });
  }

  async submitComponentData(event) {

    event.preventDefault();
    let request = `api/protected/chatrooms/create`;
    let response = await AuthService.fetch(request, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({})
    });

    if (response.ok) {

      const users = this.state.users;
      const chatroom = await response.json();

      users.forEach(async user => {
        if (user.selected) {

          request = `api/protected/memberships/create`;
          response = await AuthService.fetch(request, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ user, chatroom })
          });
        }
      });

      this.setState({ users: users.map(user => ({ ...user, selected: false })) });
    }
  }

  componentDataSelected(user, state) {
    this.setState(prevState => ({
      users: prevState.users.map(x => {
        return x.id !== user.id ? x : { ...x, selected: state }
      })
    }));
  }

  render() {
    const { ready, users } = this.state;
    const { component: Component, htmlFor } = this.props;

    if (!ready) {
      return <p><em>Loading...</em></p>
    }

    return (
      <div aria-label="chat-editor-area">
        <div className="d-flex h-100 flex-column">
          <div className="row no-gutters flex-grow-1">
            <div className="col">
              <ul className="list-group"> {
                users.map((user, idx) =>
                  <li key={`list-group-item-${idx}`}
                    className="list-group-item p-2">
                    <Component {...user}
                      componentSelectable={true}
                      componentSelected={user.selected}
                      componentSelectionChanged={state =>
                        this.componentDataSelected(user, state)} />
                  </li>
                )}
              </ul>
            </div>
          </div>
          <div className="row no-gutters flex-grow-0">
            <div className="col" style={{ height: 64 }}>
              <form className="d-flex h-100 align-items-center"
                onSubmit={(e) => this.submitComponentData(e)}>

                {users.some(user => user.selected) &&
                  <button className="btn btn-block p-0 border-0">
                    <label className="d-flex align-items-center h-100 justify-content-end m-0" 
                    htmlFor={htmlFor}>
                      <h4 className="m-0 mx-0">Create new chatroom!</h4>
                      <i className="m-0 mx-3 fas fa-lg fa-arrow-right" />
                    </label>
                  </button>}

              </form>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default ChatEditorArea;
