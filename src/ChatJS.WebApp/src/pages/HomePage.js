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

class HomePage extends Component {

  state = {
    chatroom: undefined,
    message: undefined,
  }

  componentDidMount() { 
    this.onSignalRConnection();
  }

  async onSignalRConnection() {
    NotifyService
      .start()
      .then((result) => {

      });
  }

  render() {
    return (
      <div className="container-fluid p-0">
        <div className="row row-nowrap no-gutters vh-100">

          <div className="col-12 col-sm-5 col-lg-3">
            <div className="d-flex flex-column h-100">
              <div className="row no-gutters flex-grow-0">
                <div className="col">
                  <Banner height={64} />
                </div>
              </div>
              <div className="row no-gutters flex-grow-1">
                <div className="col">
                  <ChatListArea
                    component={ChatListEntry}
                    componentSelected={e => this.setState({ chatroom: e })} />
                </div>
              </div>
              <div className="row no-gutters flex-grow-0">
                <div className="col">
                  <UserArea
                    component={ChatListEntry}
                    componentSelected={e => console.log(e)} />
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 col-sm-7 col-lg-9">
            <div className="d-flex flex-column h-100">
              <MessageArea 
                chatroom={this.state.chatroom}
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