import { Component } from "react";
import { NotifyService } from "./websockets/NotificationService";
import { AuthService } from "./api-authorization/AuthorizeService";

import './ChatListArea.css';
import './ChatListEntry.css';

import users_empty from '../assets/ico/users-empty.svg';

class ChatListArea extends Component {

  state = {
    ready: false,
    chatrooms: undefined
  }

  componentDidMount() {
    this.fetchComponentData();

    NotifyService.on("CreateChatroom", this.createComponentData);
    NotifyService.on("UpdateChatroom", this.updateComponentData);
    NotifyService.on("DeleteChatroom", this.deleteComponentData);

    NotifyService.on("CreateMembership", this.createComponentData);
    NotifyService.on("UpdateMembership", this.updateComponentData);
    NotifyService.on("DeleteMembership", this.deleteComponentData);
  }

  componentWillUnmount() {

    NotifyService.off("CreateChatroom", this.createComponentData);
    NotifyService.off("UpdateChatroom", this.updateComponentData);
    NotifyService.off("DeleteChatroom", this.deleteComponentData);

    NotifyService.off("CreateMembership", this.createComponentData);
    NotifyService.off("UpdateMembership", this.updateComponentData);
    NotifyService.off("DeleteMembership", this.deleteComponentData);
  }

  async fetchComponentData() {

    const request = `api/protected/memberships/all`;
    console.log('ChatListArea', request);

    const response = await AuthService.fetch(request);
    console.log('ChatListArea', response);

    const data = await response.json();
    console.log('ChatListArea', data);

    this.setState({ ...data, ready: true });
  }

  createComponentData = (chatroom) => {

    const { chatrooms } = this.state;
    if (!!chatrooms && chatrooms.some(x => x.id === chatroom.id)) {
      this.updateComponentData(chatroom);
    } else {
      this.setState(prevState => ({
        chatrooms: [...prevState.chatrooms, chatroom]
      }));
    }
  }

  updateComponentData = (chatroom) => {
    this.setState(prevState => ({
      chatrooms: prevState.chatrooms.map(prevChatroom => {
        return chatroom.id !== prevChatroom.id ? prevChatroom : {
          ...prevChatroom, ...chatroom
        }
      })
    }));
  }

  deleteComponentData = (chatroom) => {
    this.setState(prevState => ({
      chatrooms: prevState.chatrooms.filter(prevChatroom => {
        return chatroom.id !== prevChatroom.id
      })
    }));
  }

  render() {

    const { ready, chatrooms } = this.state;
    const { user,
      component: Component,
      componentSelected } = this.props;

    if (!ready || !user) {
      return <p><em>Loading...</em></p>
    }

    chatrooms.forEach(chatroom => {
      if (!!chatroom.members) {
        chatroom.name = chatroom.name || (chatroom.members.length !== 2
          ? chatroom.members.filter(x => x.id !== user.id).map(x => x.name).concat("You").join(", ")
          : chatroom.members.filter(x => x.id !== user.id).map(x => x.name).join(", "));

        chatroom.nameCaption = chatroom.nameCaption || (chatroom.members.length !== 2
          ? `${chatroom.members.filter(x => x.id !== user.id).length + 1} Members`
          : `${chatroom.members.filter(x => x.id !== user.id)[0].nameUid}`);
      }
    });

    return (
      chatrooms.length === 0
        ? <div aria-label="chat-list-area">
            <div className="d-flex flex-column h-100 align-items-center justify-content-center">
              <img src={users_empty} width="160" height="160" />
              <h3 className="text-muted">No conversations found</h3>
              <p className="text-muted">Try starting a conversation with other users of this site!</p>
            </div>
          </div>
        : <div aria-label="chat-list-area">
          <ul className="list-group"> {
            chatrooms.map((chatroom, idx) =>
              <li
                key={`list-group-item-${idx}`}
                className="list-group-item list-group-item-action p-2"
                onClick={() => componentSelected && componentSelected({ ...chatroom })}>
                <Component {...chatroom} />
              </li>
            )}
          </ul>
        </div>
    );
  }
}

export default ChatListArea;