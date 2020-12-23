import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './ChatListArea.css';
import './ChatListEntry.css';

class ChatListArea extends Component {

  state = {
    ready: false,
    entries: undefined
  }

  componentDidMount() {
    this.fetchComponentData();
  }

  componentDidUpdate() {
  }

  async fetchComponentData() {

    const request = `api/private/chatlogs`;
    console.log('ChatListArea', {request});

    const response = await AuthService.fetch(request);
    console.log('ChatListArea', {response});

    const data = await response.json();
    console.log('ChatListArea', {data});

    this.setState({ ...data, ready: true});
  }

  render() {
    const { ready, entries } = this.state;
    const { component: Component, componentSelected } = this.props;

    if (!ready) {
      return <p><em>Loading...</em></p>
    }

    return (
      <div aria-label='chat-list-area'>
        <ul className="list-group"> {
          entries.map((entry, idx) =>
            <div key={`list-item-${idx}`} 
              aria-label='chat-list-item'>
              <li className="list-group-item list-group-item-action p-2"
                onClick={() => componentSelected && componentSelected({...entry})}>
                <Component {...entry} />
              </li>
            </div> 
        )}
        </ul>
      </div>
    );
  }
}

export default ChatListArea;