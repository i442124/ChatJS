import { Component, Fragment } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './ChatListArea.css';
import './ChatListEntry.css';
import ChatListEntry from "./ChatListEntry";

class ChatListArea extends Component {

  state = {
    ready: false,
    entries: undefined
  }

  componentDidMount() {
    this.fetchComponentData();
  }

  async fetchComponentData() {
    const token = await AuthService.getAccessToken();
    const response = await fetch(`api/private/chatlogs`, {
      headers: !token ? {} : {'Authorization' : `Bearer ${token}` }
    });

    const data = await response.json();
    console.log(data);
    this.setState({ ...data, ready: true });
  }

  render() {
    const { ready, entries } = this.state;
    const { onRequestChatlog } = this.props;

    if (!ready) {
      return (<div>Loading...</div>);
    }

    return (
      <ul className="list-group"> {
        entries.map((entry, idx) =>
          <li key={`list-item-${idx}`} 
              className="list-group-item list-group-item-action p-2"
              onClick={() => onRequestChatlog && onRequestChatlog(entry.id)}>
              <ChatListEntry {...entry} />
          </li>
        )}
      </ul>
    );
  }
}

export default ChatListArea;