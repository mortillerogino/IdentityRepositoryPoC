import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styles: []
})
export class UserListComponent implements OnInit {

  users;

  constructor(private service: UserService) { }

  ngOnInit() {
    this.service.getUsers().subscribe(
      (res: any) => {
        this.users = res;
        console.log(this.users)
      }
    )
  }

}
