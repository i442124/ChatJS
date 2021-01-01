import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";
import { NotifyService } from "./websockets/NotificationService"

import './MessageArea.css';
import './MessageEntry.css';

class MessageArea extends Component {

  state = {
    ready: false,
    messages: undefined,
  }

  constructor() {
    super();
    this.getNewMessage = this.getNewMessage.bind(this);
  }

  shouldComponentUpdate(props) {
    const { chatroom } = this.props;

    if (!!props.chatroom && (!chatroom ||
      props.chatroom.id !== chatroom.id)) {
      console.log('Switch');
      this.fetchComponentData(props.chatroom);

      if (!!chatroom) {
        NotifyService.off("GetNewPost", chatroom.id);
      }

      NotifyService.on("GetNewPost", props.chatroom.id, this.getNewMessage);
      return false;
    }

    return true;
  }

  async fetchComponentData(chatroom) {

    const request = `api/private/chatlogs/${chatroom.id}`
    console.log('MessageArea', { request });

    const response = await AuthService.fetch(request);
    console.log('MessageArea', { response });

    const data = await response.json();
    console.log('MessageArea', { data });

    this.setState({ ...data, ready: true });
  }

  getNewMessage(message) {
    console.log("New message...");
    console.log(message);

    this.setState(prevState => ({
      messages: [...prevState.messages, message]
    }));

    console.log("Success...");
    console.log(this.state.messages);
  }

  getLocalMessages(globalMessages) {
    var currentDay = undefined;
    var previousDay = undefined;

    var localMessages = [];

    if (!!globalMessages) {
      globalMessages.forEach(message => {

        if (message.creator.id === AuthService.user.id) {
          message.origin = 'send';
        } else {
          message.origin = 'received';
        }

        currentDay = new Date(message.timeStamp).getDay();
        if (previousDay === undefined || previousDay !== currentDay) {
          localMessages.push({ content: this.getLocaleDateString(message.timeStamp) });
          previousDay = currentDay;
        }

        localMessages.push(message);
      });

      return localMessages;
    }
  }

  getLocaleDateString(timeStamp) {
    return new Date(timeStamp).toLocaleDateString(
      navigator.language, { day: 'numeric', month: 'numeric', year: 'numeric' });
  }

  render() {

    const { ready, messages } = this.state;
    const { chatroom,
      component: ComponentBody,
      componentHeader: ComponentHeader,
      componentFooter: ComponentFooter } = this.props;

    const localMessages = this.getLocalMessages(messages);

    return (
      <div aria-label="message-area">
        <div className="d-flex flex-column h-100">
          <div className="row no-gutters flex-grow-0">
            <div className="col p-2" aria-label="message-area-header">
              <ComponentHeader {...chatroom} />
            </div>
          </div>
          <div className="row no-gutters flex-grow-1">
            <div className="col p-2" aria-label="message-area-body">
              <div className="d-flex flex-column">
                {ready && localMessages.map((message, idx) =>
                  <ComponentBody key={`message-${idx}`} {...message}
                    shouldRenderName={chatroom.members.length !== 2 && 
                      !!message.creator && message.creator.id !== AuthService.user.id} />
                )}
              </div>
            </div>
          </div>
          <div className="row no-gutters flex-grow-0">
            <div className="col p-2" aria-label="message-area-footer">
              <div className="d-flex flex-column">
                <ComponentFooter chatroom={chatroom} />
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default MessageArea;