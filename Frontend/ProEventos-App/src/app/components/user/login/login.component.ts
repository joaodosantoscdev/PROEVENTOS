import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '@app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Subscriber } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  model: any = {};

  constructor(private userService: UserService,
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  public login(): void {
    this.userService.login(this.model).subscribe(
      () => { this.router.navigateByUrl('/dashboard'); },
      (error: any) => {
        if (error.status === 401) { this.toastr.error('Usuário ou senha inválido'); }
        else { console.error(error); }
      }
    );
  }

}
