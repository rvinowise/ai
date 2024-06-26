--
-- PostgreSQL database dump
--

-- Dumped from database version 14.3
-- Dumped by pg_dump version 14.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: caret; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.caret (
    moment bigint
);


ALTER TABLE public.caret OWNER TO postgres;

--
-- Name: TABLE caret; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public.caret IS 'stores the last moment in the historyt of input signals';


--
-- Name: edge; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.edge (
    parent text NOT NULL,
    tail text NOT NULL,
    head text NOT NULL
);


ALTER TABLE public.edge OWNER TO postgres;

--
-- Name: figure; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.figure (
    id text NOT NULL
);


ALTER TABLE public.figure OWNER TO postgres;

--
-- Name: figure_appearance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.figure_appearance (
    figure text NOT NULL,
    head bigint NOT NULL,
    tail bigint NOT NULL,
    CONSTRAINT "head < tail" CHECK ((head < tail))
);


ALTER TABLE public.figure_appearance OWNER TO postgres;

--
-- Name: subfigure; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subfigure (
    id text NOT NULL,
    parent text NOT NULL,
    referenced text NOT NULL
);


ALTER TABLE public.subfigure OWNER TO postgres;

--
-- Data for Name: caret; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.caret (moment) FROM stdin;
\.


--
-- Data for Name: edge; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.edge (parent, tail, head) FROM stdin;
A	Aa	Ab
A	Ab	Ac
A	Ab	Ad
A	Ad	Ae
\.


--
-- Data for Name: figure; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.figure (id) FROM stdin;
a
b
A
c
d
e
\.


--
-- Data for Name: figure_appearance; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.figure_appearance (figure, head, tail) FROM stdin;
a	1	2
a	3	4
\.


--
-- Data for Name: subfigure; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.subfigure (id, parent, referenced) FROM stdin;
Aa	A	a
Ab	A	b
Ac	A	c
Ad	A	d
Ae	A	e
\.


--
-- Name: edge edge_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT edge_pkey PRIMARY KEY (parent, tail, head);


--
-- Name: figure_appearance figure_appearance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.figure_appearance
    ADD CONSTRAINT figure_appearance_pkey PRIMARY KEY (figure, head, tail);


--
-- Name: figure figure_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.figure
    ADD CONSTRAINT figure_pkey PRIMARY KEY (id);


--
-- Name: subfigure subfigure_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT subfigure_pkey PRIMARY KEY (id);


--
-- Name: figure_appearance figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.figure_appearance
    ADD CONSTRAINT figure FOREIGN KEY (figure) REFERENCES public.figure(id);


--
-- Name: edge head is a subfigure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT "head is a subfigure" FOREIGN KEY (head) REFERENCES public.subfigure(id) NOT VALID;


--
-- Name: subfigure parent is a figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT "parent is a figure" FOREIGN KEY (parent) REFERENCES public.figure(id) NOT VALID;


--
-- Name: edge parent is a figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT "parent is a figure" FOREIGN KEY (parent) REFERENCES public.figure(id) NOT VALID;


--
-- Name: subfigure referenced is a figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT "referenced is a figure" FOREIGN KEY (referenced) REFERENCES public.figure(id) NOT VALID;


--
-- Name: edge tail is a subfigure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT "tail is a subfigure" FOREIGN KEY (tail) REFERENCES public.subfigure(id) NOT VALID;


--
-- PostgreSQL database dump complete
--

