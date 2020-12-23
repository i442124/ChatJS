import { Component } from "react";

import './MessageEntry.css';

class MessageEntry extends Component {

  render() {

    const attribute = this.getAttribute();
    const { name, content, timeStamp } = this.props;

    const shouldRenderName = !!name;
    const shouldRenderTime = !!timeStamp;

    return (
      <div className={`message-item ${attribute}`}>
       
        { shouldRenderName && 
          <div className="message-name">
            {name}
          </div> 
        }

        <div className="d-inline-flex">
          <div className="message-contents">{content}</div>
          { shouldRenderTime && 
            <div className="message-time">
              {this.getLocateTimeString()}
            </div> 
          }
        </div>
      </div>
    );
  }

  getAttribute() {
    const { origin } = this.props;
    return !!origin ? `message-item-${origin}` : ``;
  }

  getLocateTimeString() {
    const { timeStamp } = this.props;
    return new Date(timeStamp).toLocaleTimeString(navigator.language, {
      hour: 'numeric', minute: 'numeric'
    });
  }
}

export default MessageEntry;