import React, { Component } from 'react';
import { NotifyService } from '../components/websockets/NotificationService';

import Banner from '../components/Banner';
import UserArea from '../components/UserArea';
import MessageArea from '../components/MessageArea';
import MessageEntry from '../components/MessageEntry';
import ChatListArea from '../components/ChatListArea';
import ChatListEntry from '../components/ChatListEntry';
import InputArea from '../components/InputArea';

import './HomePage.css';
import ChatEditorArea from '../components/ChatEditorArea';

class HomePage extends Component {

  state = {
    ready: false,
    user: undefined,
    message: undefined,
    chatroom: undefined,
  }

  componentDidMount() {
    this.onSignalRConnection();
  }

  componentWillUnmount() {
    this.offSignalRConnection();
  }

  async offSignalRConnection() {
    await NotifyService.stop();
    this.setState({ ready: false });
  }

  async onSignalRConnection() {
    await NotifyService.start()
    this.setState({ ready: true });
  }

  render() {

    const { chatroom } = this.state;
    const { ready, user } = this.state;

    if (!ready) {
      return (<div>Loading...</div>);
    }

    return (
      <div className="container-fluid p-0">
        <div className="row row-nowrap no-gutters vh-100">

          <div className="col-12 col-sm-5 col-lg-3">
            <div className="d-flex flex-column h-100">
              <div className="row no-gutters flex-grow-0">
                <div
                  className="col"
                  style={{ background: '#efefef' }}>
                  <Banner height={64} />
                </div>
                <div
                  className="col-auto"
                  style={{ background: '#efefef' }}>
                  <div className="d-flex h-100 align-items-center">
                    <label htmlFor="create-toggle" className="m-0 mx-3">
                      <i className="fas fa-2x fa-user-plus text-muted"></i>
                    </label>
                  </div>
                </div>
              </div>

              <div className="row no-gutters flex-grow-1">
                <div className="col">
                  <ChatListArea user={user}
                    component={ChatListEntry}
                    componentSelected={e => this.setState({ chatroom: e })} />
                </div>
              </div>
              <div className="row no-gutters flex-grow-0">
                <div className="col">
                  <UserArea
                    component={ChatListEntry}
                    componentSelected={e => console.log(e)}
                    componentDataChanged={e => this.setState({ user: e })} />
                </div>
              </div>
            </div>
          </div>

          <div
            aria-label="chat-room-creator"
            className="col-12 col-sm-5 col-lg-3">
            <input hidden type="checkbox" id="create-toggle" />
            <div className="d-flex flex-column h-100">
              <div className="row no-gutters flex-grow-0">
                <div className="col col-head" style={{ height: 64 }}>
                  <div className="d-flex h-100 align-items-center">
                    <label className="m-0 mx-3" htmlFor="create-toggle">
                      <i className="fas fa-lg fa-arrow-left" />
                    </label>
                    <h4 className="m-0">Add group participants.</h4>
                  </div>
                </div>
              </div>
              <div className="row no-gutters flex-grow-1">
                <ChatEditorArea
                  user={user}
                  htmlFor={'create-toggle'}
                  component={ChatListEntry}
                  componentSelected={e => console.log(e)}>
                </ChatEditorArea>
              </div>
            </div>
          </div>

          <div className="col-12 col-sm-7 col-lg-9">
            <div className="d-flex flex-column h-100">
              <MessageArea
                user={user}
                chatroom={chatroom}
                component={MessageEntry}
                componentFooter={InputArea}
                componentHeader={ChatListEntry}
                componentSelected={e => console.log(e)} />
            </div>
          </div>

        </div>
      </div>
    );
  }
}

export default HomePage;