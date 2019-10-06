package com.example.CourceOnline;

import org.springframework.web.bind.annotation.RequestMapping;

public class Webcontroller {
	@RequestMapping("/")
	public String viewindex() {
		   return "index";
		}
}
