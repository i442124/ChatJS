import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './InputArea.css';

class InputArea extends Component {

  state = {
    content: '',
    attachment: null
  }

  componentDidMount() {
  }

  componentDidUpdate() {
  }

  async fetchComponentData() {
    let request = `api/protected/messages/create`;
    let response = await AuthService.fetch(request, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ content: this.state.content }),
    });

    if (response.ok) {

      var chatroom = this.props.chatroom;
      var message = await response.json();

      request = `api/protected/posts/create`
      response = await AuthService.fetch(request, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ chatroom: chatroom, message: message })
      });
    }

    this.setState({ content: '' });
  }

  async submitComponentData(event) {
    event.preventDefault();
    await this.fetchComponentData();
  }

  render() {

    const { content } = this.state;
    const { placeholder } = this.props;

    return (
      <form
        className="d-flex flex-column"
        onSubmit={e => this.submitComponentData(e)}>

        <div className="d-flex align-items-center">
          <div className="d-flex flex-grow-1 mx-2">
            <input
              value={content}
              placeholder={placeholder}
              className="form-control shadow-sm"
              style={{ border: 'none', borderRadius: '1rem' }}
              onChange={e => this.setState({ content: e.target.value })}>
            </input>
          </div>

          <div
            style={{ width: 48, height: 48 }}
            className="d-flex align-items-center">
            <button className="btn border-0 text-muted">
              <i className="fas fa-lg fa-paper-plane" />
            </button>
          </div>

        </div>
      </form>
    )
  }
}

InputArea.defaultProps = {
  chatroom: { id: undefined },
  placeholder: "Type a message...",
}

export default InputArea;