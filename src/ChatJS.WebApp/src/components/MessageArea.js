import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './MessageArea.css';
import './MessageEntry.css';

class MessageArea extends Component {

  state = {
    ready: false,
    id: undefined,
    name: undefined,
    caption: undefined,
    entries: undefined
  }

  componentDidMount() {
  }

  componentDidUpdate() {
    const { id } = this.props;
    this.fetchComponentData(id);
  }

  shouldComponentUpdate(props) {
    const { id } = this.state;
    return !!props.id && props.id !== id;
  }

  getLocaleMessages(entries) {

    var currentDay = undefined;
    var previousDay = undefined;
    var messages = [];

    if (!!entries) {
      entries.forEach(entry => {
        currentDay = new Date(entry.timeStamp).getDay();
        if (previousDay === undefined || previousDay != currentDay) {
          messages.push({ content: this.getLocaleDateString(entry.timeStamp)});
          previousDay = currentDay;
        }
        messages.push(entry);
      });
    }

    return messages;
  }

  getLocaleDateString(timeStamp) {
    return new Date(timeStamp).toLocaleDateString(
      navigator.language, { day: 'numeric', month: 'numeric', year: 'numeric' });
  }

  async fetchComponentData(chatlogId) {

    const request = `api/private/messages/${chatlogId}`;
    console.log('MessageArea', { request });

    const response = await AuthService.fetch(request);
    console.log('MessageArea', { response });

    const data = await response.json();
    console.log('MessageArea', { data });

    this.setState({ ...data, ready: true});
  }

  render() {
    const { ready, name, caption, entries } = this.state;
    const { component: ComponentBody, componentHeader: ComponentHeader, componentFooter: ComponentFooter } = this.props;
    const messages = this.getLocaleMessages(entries);

    return(
      <div aria-label="message-area">
        <div className="d-flex flex-column h-100">
          <div className="row no-gutters flex-grow-0">
            <div className="col p-2" aria-label="message-area-header">
              <ComponentHeader name={name} caption={caption} />
            </div>
          </div>
          <div className="row no-gutters flex-grow-1">
            <div className="col p-2" aria-label="message-area-body">
              <div className="d-flex flex-column">
                {ready && messages.map((entry, idx) =>
                  <ComponentBody {...entry} />
                )}
              </div>
            </div>
          </div>
          <div className="row no-gutters flex-grow-0">
            <div className="col p-2" aria-label="message-area-footer">
              <div className="d-flex flex-column">
                <ComponentFooter />
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default MessageArea;