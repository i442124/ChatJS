import { Component } from "react";

import './MessageArea.css';
import './MessageEntry.css';

class MessageEntry extends Component {

  render() {

    const { name, content, timeStamp, origin } = this.props;
    const messageAttribute = this.getMessageAttribute(origin);

    const shouldRenderName = !!name;
    const shouldRenderTime = !!timeStamp;

    return (
      <div className={`message-item ${messageAttribute}`}>
        { shouldRenderName &&
          <div className="message-name">
            {name}
          </div>
        }

        <div className="d-inline-flex">
          <div className="message-contents">{content}</div>
          { shouldRenderTime &&  
            <div className="message-time">
              {this.getLocaleTimeString(timeStamp)}
            </div> 
          }
        </div>
      </div>
    );
  }

  getMessageAttribute(origin) {
    return !!origin ? `message-item-${origin}` : ``;
  }

  getLocaleTimeString(timeStamp) {
    return new Date(timeStamp).toLocaleTimeString(
      navigator.language, { hour: 'numeric', minute: 'numeric'});
  }

  getLocaleDateString(timeStamp) {
    return new Date(timeStamp).toLocaleDateString(
      navigator.language, { day: 'numeric', month: 'numeric', year: 'numeric' });
  }

}

export default MessageEntry;