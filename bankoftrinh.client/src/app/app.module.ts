import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { AppComponent } from "./app.component";

@NgModule({
  declarations: [],
  imports: [BrowserModule, HttpClientModule, AppComponent],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
