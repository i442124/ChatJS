import { Component } from "react";

import './InputArea.css';

class InputArea extends Component {

  render() {
    return (
      <form className="h-100 p-2">
        <div className="d-flex align-items-center">
          <div className="d-flex flex-grow-1 mx-2">
            <input 
              style={{ borderRadius: "1rem"}}
              placeholder="Type a message..."
              className="form-control shadow-sm border-0" />
          </div>
          <div 
            style={{ height: 48, width: 48 }}
            className="d-flex justify-content-center align-items-center" >
            <i className="text-muted fas fa-lg fa-paper-plane" />
          </div>
        </div>
      </form>
    );
  }
}

export default InputArea;