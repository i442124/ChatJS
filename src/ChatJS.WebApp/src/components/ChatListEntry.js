import { Component } from "react";
import defaultUser from '../assets/ico/user.svg';

import './ChatListArea.css';
import './ChatListEntry.css';

class ChatListEntry extends Component {

  render() {

    const { selected, status,
      name, nameCaption,
      componentSelectable,
      componentSelectionChanged } = this.props;

    return (
      <div className="d-flex flex-column">
        <div className="row no-gutters align-items-center">

          <div className="col">
            <div className="row no-gutters">

              <div className="col-auto">
                <div className="position-relative mr-2">
                  <img className="rounded-circle" src={defaultUser} alt="usr_ico" width="48" height="48" />
                  <div className={`position-absolute rounded-circle ${!!status ? 'status-' + status : ''}`}
                    style={{ width: 18, height: 18, right: 0, bottom: 0 }}>
                  </div>
                </div>
              </div>

              <div className="col-auto">
                <div className="d-flex flex-column">
                  <h5 className="text-muted text-turncate m-0 font-weight-bold">{name}</h5>
                  <h6 className="text-muted text-turncate m-0 font-weight-normal">{nameCaption}</h6>
                </div>
              </div>

            </div>
          </div>

          {componentSelectable &&
            <div className="col-auto">
              <label className="chat-box m-0 mx-3">
                <i className="far fa-square"
                  style={{ transform: 'scale(1.33)' }}>

                  <input type="checkbox" checked={selected}
                    onChange={e => componentSelectionChanged(e.target.checked)}>
                  </input>

                  <span className="checkmark">
                    <i className="fas fa-check-square" />
                  </span>

                </i>
              </label>
            </div>
          }

        </div>
      </div>
    );
  }

}

export default ChatListEntry;