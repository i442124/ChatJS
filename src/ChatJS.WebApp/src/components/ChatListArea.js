import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";
import ChatListEntry from "./ChatListEntry";

class ChatListArea extends Component {

  state = {
    ready: false,
    chatlogs: undefined
  }

  render() {

    const { user } = this.props;
    const { ready, chatlogs } = this.state;

    return (
      <div className="d-flex flex-column h-100">
        <div className="row flex-grow-1">
          <div className="col">
            <ul className="list-group">
              <li className="list-group-item list-group-item-action p-2"><ChatListEntry name={"Name"} caption={"#0002"} status={"online"}/></li>
              <li className="list-group-item list-group-item-action p-2"><ChatListEntry name={"Name"} caption={"#0003"} status={"online"}/></li>
              <li className="list-group-item list-group-item-action p-2"><ChatListEntry name={"Name"} caption={"#0004"} status={"offline"}/></li>
            </ul>
          </div>
        </div>
        <div className="row flex-grow-0">
          <div className="col">
            <div className="d-flex align-items-center p-2"
              style={{backgroundColor: '#efefef'}}>
              <div className="flex-grow-1">
                <ChatListEntry name={"You"} caption={"#0001"} status={"online"}/>
              </div>
              <div className="flex-grow-0">
                <button className="btn btn-dark" onClick={() => AuthService.signOut()}>Logout</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default ChatListArea;