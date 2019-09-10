import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {

  constructor(private service: UserService, private router: Router, private toastr: ToastrService) { }

  formModel = {
    UserName: '',
    Password: ''
  }

  ngOnInit() {
    
  }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        //this.router.navigateByUrl('/home');
        this.toastr.success("TOKEN: " + res.token, "Authentication Success");
      },
      err => {
        if (err.status == 400) {
          this.toastr.error(err.error, "Authentication Failed");
        }
      }
    );
  }

}
