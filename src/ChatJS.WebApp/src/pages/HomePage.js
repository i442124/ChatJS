import React, { Component, Fragment } from 'react';

import Banner from '../components/Banner';
import ChatListArea from '../components/ChatListArea';
import MessageArea from '../components/MessageArea';

import './HomePage.css';

class HomePage extends Component {

  render() {
    return (
      <div className="container-fluid p-0">
        <div className="row row-span no-gutters vh-100">
          
          <div className="col-12 col-sm-5 col-lg-3">
           <div className="d-flex flex-column h-100">
              <div className="row no-gutters flex-grow-0">
                <div className="col">
                  <Banner height={64}/>
                </div>
              </div>
              <div className="row no-gutters flex-grow-1">
                <div className="col">
                  <ChatListArea />
                </div>
              </div>
           </div>
          </div>

          <div className="col-12 col-sm-7 col-lg-9">  
            <div className="d-flex flex-column h-100">
              <div className="row no-gutters flex-grow-0">
                <div className="col" style={{height: 64, background: '#efefef'}}>
                </div>
              </div>
              <div className="row no-gutters flex-grow-1">
                <div className="col">
                  <MessageArea />
                </div>
              </div>
           </div>
          </div>

        </div>
      </div>
    );
  }
}

export default HomePage;