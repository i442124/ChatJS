import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './InputArea.css';

class InputArea extends Component {
  
  state = {
    content: '' 
  }

  async fetchComponentData() {
    let request = `api/private/messages/create`;
    let response = await AuthService.fetch(request, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ content: this.state.content }),
    });

    if (response.ok) {

      var chatroom = this.props.chatroom;
      var message = await response.json();

      request = `api/private/posts/create`
      response = await AuthService.fetch(request, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ chatroom: chatroom, message: message })
      });
    }

  }

  render() {
    return (
      <form className="d-flex flex-column">
        <div className="d-flex align-items-center">
          <div className="d-flex flex-grow-1 mx-2">
            <input 
              value={this.state.content} 
              style={{ borderRadius: "1rem"}}
              className="form-control shadow-sm border-0"
              onChange={e => this.setState({ content: e.target.value})}>
            </input>
          </div>
          <div 
            style={{ height: 48, width: 48 }}
            onClick={e => this.fetchComponentData()}
            className="d-flex justify-content-center align-items-center">
            <i className="text-muted btn fas fa-lg fa-paper-plane" />
          </div>
        </div>
      </form>
    );
  }
}

export default InputArea;