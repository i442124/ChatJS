import { Component } from "react";
import { AuthService } from "./api-authorization/AuthorizeService";

import './MessageArea.css';
import InputArea from "./InputArea";
import MessageEntry from "./MessageEntry";

class MessageArea extends Component {

  state = {
    ready: false,
    entires: undefined
  }

  componentDidUpdate(props) {
    const { id } = this.props;
    if (props.id != id) {
      this.fetchComponentData(id);
    }
  }

  async fetchComponentData(id) {
    console.log(id);
    const token = await AuthService.getAccessToken();
    const response = await fetch(`api/private/messages/${id}`, {
      headers: !token ? {} : {'Authorization' : `Bearer ${token}` }
    });

    const data = await response.json();
    console.log(data);
    this.setState({ ...data, ready: true });
  }

  render() {

    const { ready, entries } = this.state;

    return (
      <div className="d-flex flex-column h-100">
        <div className="row no-gutters flex-grow-1">
          <div className="col" style={{ background: '#e5ddd5'}}>
            <div className="d-flex flex-column">{
              ready && entries.map((entry, idx) =>
                <MessageEntry key={idx} {...entry} />
             )}
            </div>
          </div>
        </div>
        <div className="row no-gutters flex-grow-0">
          <div className="col" style={{ background: '#efefef'}}>
            { ready && <InputArea/> }
          </div>
        </div>
      </div>
    );
  }
}

export default MessageArea;