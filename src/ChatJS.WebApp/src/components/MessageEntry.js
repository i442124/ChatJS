import { Component } from "react";

import './MessageEntry.css';

class MessageEntry extends Component {

  render() {

    const attribute = this.getAttribute();
    const { name, contents, timeStamp } = this.props;

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
          <div className="message-contents">{contents}</div>
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
    const { attribute} = this.props;
    return !!attribute ? `message-item-${attribute}` : ``;
  }

  getLocateTimeString() {
    const { timeStamp } = this.props;
    return timeStamp.toLocaleTimeString(navigator.language, {
      hour: 'numeric', minute: 'numeric'
    });
  }
}

export default MessageEntry;