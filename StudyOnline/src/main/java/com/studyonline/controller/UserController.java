package com.studyonline.controller;

import com.studyonline.entity.User;
import com.studyonline.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.List;

@Controller
public class UserController {
    @Autowired
    private UserService userService;

    @RequestMapping("/")
    public String userList(Model model){
        List<User> users = userService.getAllUser();
        model.addAttribute("user_list",users);
        return "CMS/user_list";
    }
}
