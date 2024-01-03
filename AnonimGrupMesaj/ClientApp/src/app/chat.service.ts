import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  public connection : any = new signalR.HubConnectionBuilder().withUrl("http://localhost:4200/chat").configureLogging(signalR.LogLevel.Information).build();
  constructor() {
    this.connection.on("ReceiveMessage", (user: string,
      message: string, messageTime: string)=>{
        console.log("User :", user);
        console.log("Message :", message);
        console.log("MessageTime :", messageTime);
      })
   }

  //start connection
  public async start(){
    try{
      await this.connection.start();
    }catch (error){
      console.log(error);  
    }
  }

  //Join Room
  public async JoinRoom(user: string, room: string){
    return this.connection.invoke("JoinRoom", {user, room})
  }


  //Send Messages
  public async sendMessage(message: string){
    return this.connection.invoke("SendMessage", message)
  }


  //Leave
  public async leaveChat()
  {
    return this.connection.stop();
  }
}
