import { Component, OnInit, inject } from '@angular/core';
import { ChatService } from '../chat.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
 
  chatService = inject(ChatService);
  inputMessage = "";
  messages: any[] = [];
  router = inject(Router);
  loggedInUserName = sessionStorage.getItem("user");
  ngOnInit(): void {
    this.chatService.messages$.subscribe(res =>{
      this.messages = res;
      console.log(this.messages);
    });
  }

  sendMessage(){
    this.chatService.sendMessage(this.inputMessage)
    .then(()=>{this.inputMessage = '';}).catch((err)=>{
      console.log(err);
    })
  }

  leaveChat()
  {
    this.chatService.leaveChat().then(()=>
    this.router.navigate(['welcome'])).catch((err)=>{
      console.log(err);
    })
  }
}
