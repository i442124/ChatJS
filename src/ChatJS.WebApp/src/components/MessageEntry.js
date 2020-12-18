import { Component } from "react";

import './MessageEntry.css';

class MessageEntry extends Component {

  render() {

    const { attribute } = this.props;
    const { name, contents, timestamp } = this.props;

    return (
      <div className={`message-item ${attribute ? attribute : ''} shadow-sm`}>
       
        { !!name && 
          <div className="font-weight-bold small text-muted">
            {name}
          </div>
        }

        <div className="d-inline-flex flex-wrap">
          <div className="m-1 mr-2">{contents}</div>
          { !!timestamp && 
            <div className="ml-auto small message-time-label">
              {timestamp.toLocaleTimeString(navigator.language, { hour: 'numeric', minute: 'numeric' })}
            </div>
          }  
        </div>
      </div>
    );
  }
}

export default MessageEntry;