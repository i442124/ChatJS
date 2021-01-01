import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";
import { AuthService } from "../api-authorization/AuthorizeService";

class NotificationService {

  async start() {

    const token = await AuthService.getAccessToken();
    console.log(token);

    const options = {
      transport: HttpTransportType.WebSockets,
      accessTokenFactory: () => `${token}`
    };

    this.connection = new HubConnectionBuilder()
      .withUrl("/chat", options)
      .withAutomaticReconnect()
      .build();

    await this.connection.start();
  }

  on(methodName, chatroomId, action) {
    this.connection.on(`${methodName} | ChatroomId: ${chatroomId}`, action)
  }

  off(methodName, chatroomId, actionRef) {
    this.connection.off(`${methodName} | ChatroomId: ${chatroomId}`, actionRef);
  }
}

export const NotifyService = new NotificationService();
