import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";
import { NotifyService } from "./websockets/NotificationService";

import './MessageArea.css';
import './MessageEntry.css';

class MessageArea extends Component {

  state = {
    ready: false,
    message: undefined
  }

  componentSourceAdded = (chatroom) => {
    NotifyService.onScoped("CreatePost", chatroom.id, this.createComponentData);
    NotifyService.onScoped("UpdatePost", chatroom.id, this.updateComponentData);
    NotifyService.onScoped("DeletePost", chatroom.id, this.deleteComponentData);
  }

  componentSourceDisposed = (chatroom) => {
    NotifyService.offScoped("CreatePost", chatroom.id, this.createComponentData);
    NotifyService.offScoped("UpdatePost", chatroom.id, this.updateComponentData);
    NotifyService.offScoped("DeletePost", chatroom.id, this.deleteComponentData);
  }

  shouldComponentUpdate(nextProps) {

    const { chatroom } = this.props;
    if (!!nextProps.chatroom && (!chatroom ||
      nextProps.chatroom.id !== chatroom.id)) {

      if (!!chatroom) {
        console.log('delete', chatroom);
        this.componentSourceDisposed(chatroom);
      }

      console.log('add', nextProps.chatroom);
      this.componentSourceAdded(nextProps.chatroom);
      this.fetchComponentData(nextProps.chatroom);
      return false;

    } else if (!!chatroom && !nextProps.chatroom) {
      console.log('delete', chatroom);
      this.componentSourceDisposed(chatroom);
      return true;
    }

    return true;
  }

  async fetchComponentData(chatroom) {

    const request = `api/protected/posts/all/${chatroom.id}`
    console.log('MessageArea', { request });

    const response = await AuthService.fetch(request);
    console.log('MessageArea', { response });

    const data = await response.json();
    console.log('MessageArea', { data });

    this.setState({ ...data, ready: true });
  }

  createComponentData = (message) => {
    this.setState(prevState => ({
      messages: [...prevState.messages, message]
    }));
  }

  updateComponentData = (message) => {
    this.setState(prevState => ({
      messages: prevState.messages.map(prevMessage => {
        return message.id !== prevMessage.id ? prevMessage : message;
      })
    }));
  }

  deleteComponentData = (message) => {
    this.setState(prevState => ({
      messages: prevState.messages.filter(prevMessage => {
        return message.id !== prevMessage.id;
      })
    }))
  }

  getLocaleMessages(globalMessages) {

    let currentDay = undefined;
    let previousDay = undefined;

    const localMessages = [];
    const { user } = this.props;

    if (!!globalMessages) {
      globalMessages.forEach(message => {
        const { creator, timeStamp } = message;

        if (creator.id === user.id) {
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
    const { chatroom, user,
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
                      !!message.creator && message.creator.id !== user.id} />
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