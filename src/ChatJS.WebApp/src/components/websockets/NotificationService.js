import { AuthService } from "../api-authorization/AuthorizeService";
import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";

class NotificationService {

  async start() {

    const token = await AuthService.getAccessToken();

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

  on(methodName, action) {
    this.connection.on(`${methodName}`, action)
  }

  onScoped(methodName, scope, action) {
    this.connection.on(`${methodName} | Scope: ${scope}`, action)
  }

  off(methodName, action) {
    this.connection.off(`${methodName}`, action)
  }

  offScoped(methodName, scope, action) {
    this.connection.off(`${methodName} | Scope: ${scope}`, action);
  }
}

export const NotifyService = new NotificationService();
