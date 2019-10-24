package com.studyonline.controller;


import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
public class WebController {
    @RequestMapping("/subject-editing")
    public String subjectEditing(){

        return "CMS/subject-editing";
    }
    @RequestMapping("/questions-list")
    public String questionsList(){

        return "CMS/questions-list";
    }
    @RequestMapping("/questions-details")
    public String questionsDetails(){

        return "CMS/questions-details";
    }
    @RequestMapping("/lesson-details")
    public String lessonDetails(){

        return "CMS/lesson-details";
    }
    @RequestMapping("/subject-adding")
    public String subjectAdding(){

        return "CMS/subject-adding";
    }
    @RequestMapping("/post-details")
    public String postDetails(){

        return "CMS/post-details";
    }
}
