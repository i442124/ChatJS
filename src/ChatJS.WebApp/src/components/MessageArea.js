import { Component } from "react";

import './MessageArea.css';
import MessageEntry from "./MessageEntry";

class MessageArea extends Component {

  render() {
    return (
      <div className="d-flex flex-column h-100">
        <div className="row no-gutters flex-grow-1">
          <div className="col" style={{ background: '#e5ddd5'}}>

            <div className="d-flex flex-column">
              <MessageEntry attribute="send" contents="This message is send" timeStamp={new Date()}/>
              <MessageEntry attribute="received" contents="This message is received" timeStamp={new Date()}/>
              <MessageEntry attribute="received" contents="This message has name" name="The author" timeStamp={new Date()}/>
              <MessageEntry contents="This message was send from the system"/>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default MessageArea;