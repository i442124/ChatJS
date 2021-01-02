import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";
import { NotifyService } from "./websockets/NotificationService"

import './MessageArea.css';
import './MessageEntry.css';

class MessageArea extends Component {

  state = {
    ready: false,
    message: undefined
  }

  componentSourceAdded = (chatroom) => {
    NotifyService.on("CreatePost", chatroom.id, 
      message => this.createComponentData(message));
    NotifyService.on("UpdatePost", chatroom.id, 
      message => this.updateComponentData(message));
    NotifyService.on("DeletePost", chatroom.id, 
      message => this.deleteComponentData(message));
  }

  componentSourceDisposed = (chatroom) => {
    NotifyService.off("CreatePost", chatroom.id,
      message => this.createComponentData(message));
    NotifyService.off("UpdatePost", chatroom.id,
      message => this.updateComponentData(message));
    NotifyService.off("DeletePost", chatroom.id,
      message => this.deleteComponentData(message));
  }

  shouldComponentUpdate(nextProps) {

    const { chatroom } = this.props;
    if (!!nextProps.chatroom && (!chatroom ||
      nextProps.chatroom.id !== chatroom.id)) {

      if (!!chatroom) {
        this.componentSourceDisposed(chatroom);
      }

      this.componentSourceAdded(nextProps.chatroom);
      this.fetchComponentData(nextProps.chatroom);
      return false;

    } else if (!!chatroom && !nextProps.chatroom) {
      this.componentSourceDisposed(chatroom);
      return true;
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

  createComponentData(message) {
    this.setState(prevState => ({
      messages: [...prevState.messages, message]
    }));
  }

  updateComponentData(message) {
    this.setState(prevState => ({
      messages: prevState.messages.map(prevMessage => {
        return message.id !== prevMessage.id ? prevMessage : message;
      })
    }));
  }

  deleteComponentData(message) {
    this.setState(prevState => ({
      messages: prevState.messages.filter(prevMessage => {
        return message.id !== prevMessage.id;
      })
    }))
  }

  getLocaleMessages(globalMessages) {

    let currentDay = undefined;
    let previousDay = undefined;
    const localMessages = new Array();

    if (!!globalMessages) {
      globalMessages.forEach(message => {
        const { creator, timeStamp } = message;

        if (creator.id === AuthService.user.id) {
          message.origin = 'send';
        } else {
          message.origin = 'received';
        }

        currentDay = new Date(timeStamp).getDay();
        if (previousDay === undefined || previousDay !== currentDay) {
          localMessages.push({ content: this.getLocaleDateString(timeStamp) });
          previousDay = currentDay;
        }

        localMessages.push(message);
      });
    }

    return localMessages;
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

    const localMessages = this.getLocaleMessages(messages);

    return (
      <div aria-label="message-area">
        <div className="d-flex flex-column h-100">
          <div className="row no-gutters flex-grow-0">
            <div className="col p-2" aria-label="message-area-header">
              <ComponentHeader {...chatroom} />
            </div>
          </div>
          <div className="row no-gutters scrollable flex-grow-1">
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